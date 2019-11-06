import tkinter as tk
from os import startfile
from tkinter import Frame

def launch(event = None):
    startfile("SOTT.exe")

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
        '''
        self.w = tk.Scale(self.root, from_=0, to=100, orient=tk.HORIZONTAL, width=12)
        self.w.grid(row = 2, column = 4)

        self.w = tk.Scale(self.root, from_=0, to=100, orient=tk.HORIZONTAL, width=12)
        self.w.grid(row = 4, column = 4)

        self.w = tk.Scale(self.root, from_=0, to=100, orient=tk.HORIZONTAL, width=12)
        self.w.grid(row = 6, column = 4)

        self.w = tk.Scale(self.root, from_=0, to=100, orient=tk.HORIZONTAL, width=12)
        self.w.grid(row = 8, column = 4)

        self.w = tk.Scale(self.root, from_=0, to=100, orient=tk.HORIZONTAL, width=12)
        self.w.grid(row = 10, column = 4)
        '''

        l = tk.Label(self.root, text="LAUNCH", background="green", width=100, height=4)
        l.grid(row = 16, column = 4)
        l.bind('<Button-1>', launch)

        s = tk.Label(self.root, text="SAVE", background="yellow", width=10, height=3)
        s.grid(row = 16, column = 5)
        s.bind('<Button-1>', self.save)

        c = tk.Label(self.root, text="CANCEL", background="red", width=10, height=3)
        c.grid(row = 16, column = 3)
        c.bind('<Button-1>', self.cancel)



        for col in range(self.root.grid_size()[0]):
            self.root.grid_columnconfigure(col, minsize=20)
        for row in range(self.root.grid_size()[1]):
            self.root.grid_rowconfigure(row, minsize=20)



    def save(self, event = None):
        print(self.w.get())

    def cancel(self, event = None):
        pass



config = {
    "background": "#3a342f"
}

root = window(config)