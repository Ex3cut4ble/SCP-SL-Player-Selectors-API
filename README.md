# Briefly
API for SMod2 plugin developers to allow admins to use player selectors.
# What can this thing do?
If you familiar with Minecraft commands, you should get the point. This API allows to use expressions, that are selecting players by some parameter, e.g. by role, team, admin rank, name, tag (plugin developer should make his own implementation of this selector, example below), or just choose players randomly.
# Show me examples
Let's say we have a command that sets the players' HP. Then its syntax will look like this: <b>sethp [Player(s)] [Amount]</b>.  
First parameter accepts some player or group of players. This is when selectors come in.  
***
***Usual selection:***  
  
**sethp 5 69** - *Finds a player with ID 5, and sets his HP to 69.*  
**sethp Execut4ble 69** - *Finds a player with name "Execut4ble", and sets his HP to 69.*  
**sethp 76561190123456789@steam 69** - *Finds a player with specific steam id, and sets his HP to 69.*  
**sethp 394049450123456789@discord 69** - *Same as previous, but with discord id.*  
**sethp * 69** - *Sets all players HP to 69.*  
***
***Expression selection:***  
  
**sethp role(\*,1) 69** - *From all (\*) players selects players with role id 1, and sets their HP to 69.*  
**sethp team(\*,3) 69** - *From all (\*) players selects players with team id 3, and sets their HP to 69.*  
**sethp rand(\*,5) 69** - *From all (\*) players selects 5 random players, and sets their HP to 69.*  
**sethp tag(\*,murder) 69** - *From all (\*) players selects players with tag "murder", and sets their HP to 69.*  
**sethp rand(role(\*,1),3) 69** - *From all (\*) players selects players with role id 1, then from these (role(\*,1)) players selects 3 random players, and sets their HP to 69.*  
**sethp rand(tag(team(\*,1),gigachad),1) 69** - *From all (\*) players selects players with team id 1, then from these (team(\*,1)) players selects players with tag "gigachad", then from these (tag(team(\*,1),gigachad)) players selects 1 random player, and sets his HP to 69.*  
# How can I use this in my plugin?
1. Download .dll file in "Releases" section, or download source code and compile it.
2. Add .dll file in your project references.
3. Create singleton instance of PlayerSelector class (and preferably provide tag predicate to its constructor, to make the tag() selector work).
4. Whenever you want to have some player group to be affected by your command, use PlayerSelector::Select(string arg) method, and put in "arg" expression string.
5. This method returns System.Collections.Generic.List<Smod2.API.Player> list with players if the expression was valid, otherwise throws an exception.
# How I should implement "tag" selector?
The way I make this in my plugins is next:  
1. I create some dictionary, which contains player id as a key, and string (the tag itself) as a value. When player joins, I add him to the dictionary with default null tag.
2. In OnEnable() method I create singleton instance of PlayerSelector class, and provide to its constructor lambda expression, e.g.:  
```c#
(targets, tag) =>
{
  List<Smod2.API.Player> output = new List<Smod2.API.Player>();
  foreach (Smod2.API.Player target in targets)
    if (playersDictionary.TryGetValue(target.PlayerId, out string playerTag) && playerTag == tag)
      output.Add(target);
  return output;
}
```
**If you set predicate to null, tag() selector will not work!**
# Can I suggest changes to this repository?
Of course, if you want to clean or extend this code, I would appreciate it.
# Does this thing work with EXILED?
No, but you can modify this thing to work with this API, it should be easy enough (if it is, you might wanna create a fork).
