package provide validation 1.0

namespace eval ::validation:: {
    namespace export isIP
    namespace export isVersion
}

proc ::validation::isIP {str type} {
   set ipnum {\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]}
   set fullExp {^($ipnum)\.($ipnum)\.($ipnum)\.($ipnum)$}
   set partialExp {^(($ipnum)(\.(($ipnum)(\.(($ipnum)(\.(($ipnum)?)?)?)?)?)?)?)?$}
   set fullExp [subst -nocommands -nobackslashes $fullExp]
   set partialExp [subst -nocommands -nobackslashes $partialExp]
   if [string equal $type focusout] {
      if [regexp -- $fullExp $str] {
         return 1
      } else {
         return 0
      }
   } else {
      return [regexp -- $partialExp $str]
   }
}

proc ::validation::isVersion {str type} {
   set fullExp {^1\.[23]\d$}
   set partialExp {^1?\.?[23]?\d?$}
   set fullExp [subst -nocommands -nobackslashes $fullExp]
   set partialExp [subst -nocommands -nobackslashes $partialExp]
   if [string equal $type focusout] {
      if [regexp -- $fullExp $str] {
         return 1
      } else {
         return 0
      }
   } else {
      return [regexp -- $partialExp $str]
   }
}
