# ConcussionWeightAdjustment

**This mod is obsolete.** The game has been patched by the publisher. This is now something that can be configured through the `simulation` settings.

A library mod for [Phantom Brigade](https://braceyourselfgames.com/phantom-brigade/) that uses only body mass to find the unit weight class adjustment for concussion damage.

It is compatible with game version **1.1.2-b5993** (Epic/Steam).

Weight classes are used to look up various adjustments to concussion/stagger damage as well as determine the outcome when two units collide. A unit's weight class is based on its total mass.

For example, if you load out a mech dressed in Tsubasa armor with a UH MG and a secondary axe, it'll be in the medium weight class and therefore have a different concussion adjustment than the same mech with only a Stand handgun which will be in the light weight class.

This mod changes the mass used to find a unit's weight class for the concussion damage adjustment factor. Instead of using the total mass, which includes the mass of any weapons, it uses only the unit's body mass.

It does not change the unit's weight class for any other calculation.
