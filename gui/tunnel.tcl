package provide tunnel 1.0
package require utils 1.0

namespace eval ::tunnel:: { namespace export start }

proc StopDialog { w } {
	return [expr [tk_messageBox -message "Stop?" -type yesno -parent $w] eq "yes"]
}

proc kill { pid } {
   if { $::tcl_platform(platform) eq "windows" } {
      set cmd "taskkill.exe /f /fi \"pid eq $pid\" 2>nul"
      exec -ignorestderr -- {*}$cmd 2>@1
   } else {
      exec kill $pid
   }
}

proc kill_channel { c } {
	if {[file channels $c] == ""} { return }
	set pid [pid $c]
	close $c
	if {$pid != ""} { kill $pid }
}

proc set_mac_quit { handler } {
	if { $::tcl_platform(os) != "Darwin" } { return }
	proc ::tk::mac::Quit {} $handler
}

proc set_mac_modified { w modified } {
	if { $::tcl_platform(os) != "Darwin" } { return }
	wm attributes $w -modified $modified
}

proc get_binary {} {
	if { $::tcl_platform(os) eq "Darwin" } {
		return [file normalize [file join [info nameofexecutable] "../../SharedSupport/bin/wc3proxy"]]
	}
	if { $::tcl_platform(platform) eq "windows" } {
		return [file normalize [file join [file dirname [info script]] ".." "wc3proxy-cli.exe"]]
	}
	return [file join [file dirname [info script]] "wc3proxy"]
}

proc closeLogWindow { stdout } {
	if { [StopDialog .log] } {
		set_mac_quit [list exit]
		kill_channel $stdout
		update
		wm withdraw .log
		destroy .log
		update
		return 1
	}
	return 0
}

proc handleQuit { stdout } {
	if {[closeLogWindow $stdout]} { exit }
}

proc handleClose { stdout } {
	if {[closeLogWindow $stdout]} { 
		wm state . normal
		wm deiconify .
		raise .
	}
}

proc ::tunnel::setup { ip stdout } {
	toplevel      .log
	wm title      .log "WC3 Proxy ($ip)"
	wm protocol   .log WM_DELETE_WINDOW [list handleClose $stdout]
	set_mac_modified .log true
	set_mac_quit [list handleQuit $stdout]

	::utils::centralize .log
	return .log
}

proc ::tunnel::scrollable { parent } {
	ttk::scrollbar $parent.vsb -orient vertical -command [list $parent.text yview]
	text $parent.text -yscrollcommand [list $parent.vsb set] 
	grid $parent.text -row 0 -column 0 -sticky nsew
	grid $parent.vsb -row 0 -column 1 -sticky nsew
	grid columnconfigure $parent 0 -weight 1
	grid rowconfigure $parent 0 -weight 1
	return $parent.text
}

proc ::tunnel::receive { window scroll stdout } {
	if {![winfo exists $scroll]} {
		kill_channel $stdout
		return
	}
	$scroll configure -state normal
	$scroll insert end [read $stdout]
	$scroll configure -state disabled
	$scroll see end
	if {[eof $stdout]} {
		kill_channel $stdout
		set_mac_modified $window false
		bell
	}
}

proc ::tunnel::start { ip version expansion } {
	set command "\"[get_binary]\" \"$ip\" \"$version\" [expr {$expansion ? "TFT" : "RoC"}]"
	set stdout [open |[concat $command 2>@1]]
	set window [::tunnel::setup $ip $stdout]
	set scroll [::tunnel::scrollable $window]
	fconfigure $stdout -blocking 0
	fileevent $stdout readable [list ::tunnel::receive $window $scroll $stdout]
}
