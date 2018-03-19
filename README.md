# User Gui LibrarY - UGLY

UGLY is small wrapper for existing GUI libraries to make creating new components easier. The main goal is to create complete abstraction from backend so that created controls are completely platform independent. This library is NOT created to replace any of existing GUI libraries, it is to ASSIST when creating interfaces. It is designed mainly for interactive, animated controls that often contains movable content (see examples: color picker, tree) and requires custom rendering.

![Color picker](https://i.imgur.com/ABhHDab.png)

![Tree view](https://i.imgur.com/JZxdLAQ.png)

The important principle is speed - the library is designed for rather fast drawing (not for games though). Currently main supported backend is Microsoft GDI+. Special thanks to [Paint.net](https://www.getpaint.net/contact.html) team and their work on fast GDI rendering. 