GEHelper
========

Android helper app for Galactic Empires game, a popular oGame clone.  

This is written in Xamarin.  I use RestSharp for networking, ServiceStack.Text for the JSON handling.

Pretty simple.  You still need the game to play - this is meant as a way to keep an eye on your overall production and do more macro-management and less micro.  Here are a few things I am planning to add:

Auto-resource Drop
Ability to send resouces to a planet or moon without having to say where they come from.  The system will just pick the closest planets and the most resouces, ships, etc.

Batch create
Specify the ratios you want for ships or defenses and then queue X blocks of those resources, potentially on all planents.

Staged attack
Send ships to attack without having to specify what planet they come from.  It will select the ships that can make it from the closest planet(s), stage them at the nearest moon or planet you own, and then launch all at once.

Any other ideas, happy to add.
