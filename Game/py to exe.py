#Hey! I wrote this about a year ago and have literally no idea how it manages to work (also i didn't write comments for some reason)

from os import system as os
from os import chdir as cd
from os import path as locate
from shutil import rmtree as delfolder
from sys import argv
os("color 0a")
#path = argv[1]
path = input("PATH: ")
path = list(path)
for i in range(len(path) - 1,0,-1):
	if path[i] == "\\":
		temp = i
		break
name = []
path[i] = ""
for i in range(temp + 1,len(path)):
	name.append(path[i])
	path[i] = ""
path = ''.join(path)
name = ''.join(name)
while True:
	if locate.exists(path + "\\" + name):
		break
	else:
		os("cls")
		print("That file does not exist.")
cd(path)
name = '"' + name + '"'
temp = "pyinstaller --onefile " + name
os(temp) 
name = list(name)
name[0] = ""
name[len(name) - 1] = ""
name[len(name) - 2] = ""
name[len(name) - 3] = ""
name[len(name) - 4] = ""
name = ''.join(name)
temp = "copy " + '"' + path + "\\dist\\" + name + ".exe" + '" ' + '"' + path + '"'
os(temp)