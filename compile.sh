#!/bin/bash

if [ ! -f /usr/lib/pkgconfig/gtk-sharp.pc ]; then 
    sudo ln -s /usr/lib/pkgconfig/gtk-sharp-2.0.pc /usr/lib/pkgconfig/gtk-sharp.pc
fi

CSFILES="NoteOfTheDay.cs NoteOfTheDayApplicationAddin.cs NoteOfTheDayPreferences.cs NoteOfTheDayPreferencesFactory.cs"

gmcs -debug -out:NoteOfTheDay.dll -target:library -pkg:tomboy-addins -r:Mono.Posix \
    $CSFILES -resource:NoteOfTheDay.addin.xml -pkg:gtk-sharp $@
