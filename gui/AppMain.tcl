package require setup
package require validation
package require tunnel

proc validateIp {str event} {
	set ::validIp [::validation::isIP $str focusout]
	return [::validation::isIP $str $event]
}
proc validateVersion {str event} {
	set ::validVersion [::validation::isVersion $str focusout]
	return [::validation::isVersion $str $event]
}
proc openTunnel {} {
	wm withdraw .
	::tunnel::start $::IP $::VERSION $::EXPANSION
}

set IP "1.0.0.1"
set VERSION "1.28"
set EXPANSION 1
set validIp 1
set validVersion 1

ttk::frame .controls -padding 4
ttk::frame .controls.top -padding {0 2 0 0}
ttk::frame .controls.bottom -padding {0 4 0 2}


ttk::entry .controls.top.1 -validate all -validatecommand {validateIp %P %V} -textvariable IP
ttk::entry .controls.top.2 -validate all -validatecommand {validateVersion %P %V} -textvariable VERSION -width 4

pack .controls.top.1 -side left -fill x -expand true
pack .controls.top.2 -side right
pack .controls.top -fill x

ttk::frame .controls.bottom.1
ttk::radiobutton .controls.bottom.1.1 -variable EXPANSION -text "RoC" -value 0
ttk::radiobutton .controls.bottom.1.2 -variable EXPANSION -text "TFT" -value 1
pack .controls.bottom.1.1 .controls.bottom.1.2 -side left -ipadx 4

ttk::button .controls.bottom.2 -text start -command openTunnel 

pack .controls.bottom.1 -side left -fill x -anchor w
pack .controls.bottom.2 -side right
pack .controls.bottom -anchor w -fill x

pack .controls -fill x

# single window provisions
ttk::frame .output -padding 0
pack .output -fill both -expand true

update
wm minsize . [winfo reqwidth .] [winfo reqheight .]
wm maxsize . 570 420
wm deiconify .

proc focusAndSelect {w} {
	focus $w
	$w select range 0 [string length [$w get]]
}

proc buttonState {} {
	return [expr {[expr $::validIp && $::validVersion] ? "active" : "disabled"}]
}

proc validIpLoop {} {
	::.controls.top.1 configure -foreground [expr {$::validIp ? "black" : "red"}]
	::.controls.bottom.2 configure -state [buttonState]
	vwait ::validIp
	after 0 validIpLoop
}
proc validVersionLoop {} {
	::.controls.top.2 configure -foreground [expr {$::validVersion ? "black" : "red"}]
	::.controls.bottom.2 configure -state [buttonState]
	vwait ::validVersion
	after 0 validVersionLoop
}

focusAndSelect .controls.top.1
validIpLoop
validVersionLoop

