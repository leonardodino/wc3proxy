package provide utils 1.0

namespace eval ::utils:: {
	namespace export centralize
}

proc ::utils::center { w } {
	set x [expr { ( [winfo vrootwidth  $w] - [winfo reqwidth $w]  ) / 2 }]
	set y [expr { ( [winfo vrootheight $w] - [winfo reqheight $w] ) / 2 }]
	wm geometry $w +${x}+${y}
	wm deiconify $w
}

proc ::utils::centralize { x } {
	wm withdraw $x
	after idle ::utils::center $x
}
