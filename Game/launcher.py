import tkinter as tk
from os import system
from os import startfile
from tkinter import Frame

def launch(event = None):
    startfile("SOTT.exe")

def close(event = None):
    system("taskkill /f /im SOTT.exe")

class window(Frame):
    def __init__(self, config):
        self.root = tk.Tk()
        self.config = config
        self.initialize()
        self.root.mainloop()

    def initialize(self):
        self.root.title("SOTT")
        self.root.geometry("960x540+100+100")
        #self.root.iconbitmap("icon.png")   # TODO
        self.root.configure(background=self.config["background"])
        self.root.resizable(0, 0)

        self.widgets()


    def widgets(self):

        l = tk.Label(self.root, text="LAUNCH", background="green", width=100, height=4)
        l.grid(row = 16, column = 4)
        l.bind('<Button-1>', launch)

        c = tk.Label(self.root, text="EXIT", background="red", width=10, height=3)
        c.grid(row = 16, column = 3)
        c.bind('<Button-1>', close)



        for col in range(self.root.grid_size()[0]):
            self.root.grid_columnconfigure(col, minsize=20)
        for row in range(self.root.grid_size()[1]):
            self.root.grid_rowconfigure(row, minsize=20)





config = {
    "background": "#3a342f"
}

root = window(config)