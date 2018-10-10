@echo EtcGenerator kingchild@163.com
@echo %1
@echo %2
@echo %3
@echo %4
@echo %5
@echo %6
@echo %7
@echo %8
@set pngPath=%1
@set pngFolderPath=%2
@set pkmPath=%3
@set alphaPkmPath=%4
@set pkmName=%5
@set alphaPkmName=%6
@set etcToolPath=%7
@set speed=%8

cd /d %etcToolPath%
@echo Convert PNG to PKM without alpha channel and solo alpha PKM files
etcpack %pngPath% %pngFolderPath% -c etc1 -s %speed% -as -progress
@echo Convert PKM files to PNG files
etcpack %pkmPath% %pngFolderPath% -ext PNG
etcpack %alphaPkmPath% %pngFolderPath% -ext PNG
@echo Remove PKM files
cd /d %pngFolderPath%
del %pkmName% /f
del %alphaPkmName% /f
@echo Life is short, coding hard.