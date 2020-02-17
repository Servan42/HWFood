# Hyrule Warriors : My Fairy Food Samples Generator

## Description
This windows console program is meant to theory craft the food system of the "My Fairy" menu of the game "Hyrule Warriors". 
The goal is to find the best food sample (in terms of mean and standard deviation) to get close to the stats required to unlock the spell "Magic Fountain+".
In consequence, are covered only the following fairy traits: Aspiring, Valiant, Shrewd, Eager, Relaxed.

## Functions
This simulator is still a work in progress and for now contains only the following functions:
* Search of the sample with the best standard deviation through a reject algorithm.
* Search of the sample with the best standard deviation AND mean through a reject algorithm.
* Search of the sample matching specific constraints through a reject algorithm.
* Generate a food sample with the best standard deviation, from food with good stats for those traits, through a recursive function with customisable lookeahed, taking into account the base stat of the fairy and the number of fairy essence already given.

## How to use
Still a WIP so no menu has been implemented. You have to hardcode the function you want to use inside the Program.cs file. 

The food database is a CSV that you can find in the repo, that allows comments (line starting with a #), containing a food name, its element, and the traits boosts it does. This info comes from [here](https://www.puissance-zelda.com/17-Hyrule_Warriors/astuces/191-La-Cantine#ravive) and [here](https://zelda.gamepedia.com/My_Fairy), and is incomplete. You have to specify the path of the CSV file into the App.config file. 

In order to use the last function stated above (recursive SD gen with lookahead) you have to specify a few parameters into the App.config file.