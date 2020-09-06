package require utils 1.0
package provide setup 1.0

wm title      . "WC3 Proxy"
wm resizable  . 1 0
wm protocol   . WM_DELETE_WINDOW {
	wm withdraw .
	exit
}
if { $::tcl_platform(os) eq "Linux" } { ttk::style theme use clam }

::utils::centralize .

# proc ::tk::mac::ShowPreferences {} {
#     bell
# }

# ::tk::mac::standardAboutPanel

