# Briefly
API for SMod2 plugin developers to allow admins to use player selectors.
# What can this thing do?
If you familiar with Minecraft commands, you should get the point. This API allows to use expressions, that are selecting players by some parameter, e.g. by role, team, admin rank, name, tag (plugin developer should make his own implementation of this selector, example below), or just choose players randomly.
# Show me examples
Let's say we have a command that sets the players' HP. Then its syntax will look like this: <b>sethp <Player(s)> <Amount of HP></b>
First parameter accepts some player or group of players. This is when selectors come in.
<b>sethp 5 69</b> - <i></i>
# How can I use this in my plugin?
1. Download .dll file in "Releases" section, or download source code and compile it.
2. Add .dll file in your project references.
3. Whenever you want to have some player group to be affected by your command, use PlayerSelector.Select(string arg) method, and put in "arg" expression string.
4. This method returns System.Collections.Generic.List<Smod2.API.Player> list with players if the expression was valid, otherwise throws an exception.
# How I should implement "tag" selector?

# Can I modify this repo?
Of course, if you want to clean this code, I would appreciate it.
