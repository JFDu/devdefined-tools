# Introduction #

This is a little project I work on every now and then, basically it's an MVP implementation for Windows Forms 2.0 leveraging the goodness of the Windsor container.  It targets the .Net Framework 2.0 (rather then 3.5) because I plan to use it to restructure a couple of existing winform's applications that are .Net Framework 2.0 only.  It's current status is "prototype" - virtually no test fixtures, but as it approaches being used in production it will hopefully mature a little.

Code can be found here:

http://devdefined-tools.googlecode.com/svn/trunk/projects/winforms-mvp/

# Details #

  * MVP Implementation with dependency injection.
  * Simple strongly typed event publisher.
  * Command Pattern implementation.
  * Uses attributes to map commands to menu items in main menus, tool button strips, context menus etc.
  * Simple but quite powerful menu hierarchy.
  * Has a simple add-in framework (incomplete) with the idea that the framework will provide a simple way to manage enabling/disabling individual add-ins to be loaded at startup.
  * Uses code for configuration of the IoC container, with a blocks of IoC configuration grouped into modules.
  * Dockable content, with attributes used to declare default positions for dockable content.
  * Default layout of dockable components is entirely declarative.