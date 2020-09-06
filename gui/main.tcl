package require Tk
package require starkit

lappend ::auto_path [file dirname [info script]]
if {[starkit::startup] ne "sourced"} { source [file join $starkit::topdir AppMain.tcl] }
