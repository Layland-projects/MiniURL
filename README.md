# MiniURL
A light weight Web API to allow you to shorten URLs

### Features:

* Quick create link
  * You can enter the url that you want to be abbreviated and MiniURL will create the new short link for you
* Different user levels
  * Guest - Links live for 1 day, no need to sign up
  * Regular - Links live for 5 days
    * This could be extended to give them visibility on their current links etc.
  * Premium - Links never expire
    * This could be extended to give them visibility on their current links, amount of traffic passing through them etc.
  * You can change a users level (to facilitate going from regular to premium for instance, and back again)
* User create link
  * Same as quick create link but it goes from a signed up user so it can be referenced back to them
* Add new user
  * Conceptual if nothing else as not auth has been implemented

### Potential expansion

* I think the main place this could be extended to convey a more premium product would be the ability to track the amount of traffic through a given link
* More management features for existing links could be good
* Ability for a certain user level (& above) to be able to choose a custom reference for their link
* Further utilisation of swagger to make the API documentation better, currently it doesn't expose all possible response types an endpoint can send which would make it harder for a front end dev to work with

