# ![HyperQube](https://raw.githubusercontent.com/StevenThuriot/HyperQube/master/SquaredDisplay.png) 

[![Build status](https://ci.appveyor.com/api/projects/status/f9sy6bfpfc6dnoyu)](https://ci.appveyor.com/project/StevenThuriot/hyperqube)

HyperQube is(/started as) an IFTTT variant, built on top of PushBullet, created for the desktop.

It connects to PushBullet's websocket using the API key provided in your account settings. It filters out the messages the plugins are interested in using [Reactive Extensions](https://github.com/Reactive-Extensions).

Deep down, everything is built completely modular so each part is easy to replace by another component (loaded by MEF). (e.g. input can easily be replaced by another UI, in WPF, WinForms, ...)
By default, everything is built with an eye on maximum compatibility. ( .NET / Mono )

#Building plugins
A sample project has been set up [here](https://github.com/steventhuriot/hyperqube-plugins).

First, create a new project. All you need to start is a reference to `HyperQube.Library`. To make life easy, a nuget package is available!

```powershell
Install-Package HyperQube.Library
```

Building a plugin is as simple as implementing an interface. 

```csharp
public interface IQube
{
  string Title { get; } //Textual reference used throughout the project.

  Interests Interests { get; } //Things you are interested in. This is a flags enum.

  void Receive(dynamic json); //Triggers each time a push message is received.
}
```

Create a new class for your plugin and simply implement the interface.

```csharp
class Qube : IQube
{
  public string Title
  {
    get { return "My First Plugin" };
  }
  
  public Interests Interests 
  {
    get { return Interests.Note | Interests.Mirror; }
  }
  
  public void Receive(dynamic json)
  {
    //json contains the full push message in json format, as pushed by PushBullet. 
    //http://docs.pushbullet.com/v2/pushes/

    //This can be used just like you would do in javascript, using dynamics.
    
    //To get the actual message, a helper is available.
    //This, in return, is a json object.
    var jsonMessage = Push.GetBody((object)json);
    
    if (jsonMessage != null)
    {
      string message = jsonMessage.ToString();
      //Do something with your newly acquired message!
    }
  }
}
```


This will do for basic cases, but in most cases you'll need to get some I/O going.
Since HyperQube uses `MEF` to build up its infrastructure, you can simply define a constructor with the parameters you need.

```csharp
[ImportingConstructor]
public Qube(IInputProvider inputProvider, IOutputProvider outputProvider)
{

}
```

The output provider can trace messages (e.g. logging) or visually show messages.
In it's default state, using the `Write` method of the output provider will show a tooltip coming from the system tray.

The input provider can be used to ask the user about some variables in your plugin. In the sample project, this is used to ask the user about their XBMC set up and what exactly they want the plugin to do.

The user can be asked about these things by using Questions. A question is a class implementing `IQuestion`. A few default [scenario's](https://github.com/StevenThuriot/HyperQube/tree/master/HyperQube.Library/Questions) have been implemented already.

* CheckBox / Toggle ([BooleanQuestion](https://github.com/StevenThuriot/HyperQube/blob/master/HyperQube.Library/Questions/BooleanQuestion.cs))
* ComboBox ([SelectableQuestion](https://github.com/StevenThuriot/HyperQube/blob/master/HyperQube.Library/Questions/SelectableQuestion.cs))
* TextBox / PasswordBox ([TextQuestion](https://github.com/StevenThuriot/HyperQube/blob/master/HyperQube.Library/Questions/TextQuestion.cs))

A question can have two types of validation.

* Required field (semi-automatic by setting a simple boolean)
* Aditional validation by implementing `IValidatableQuestion`
  * This interface has a property that supplies a list of `IValidation` instances.
  * A base class has been supplied to easily implement a validation: `Validation`
    * A validation contains a predicate that returns true if the value in the box is valid.
    * It also contains a message to show when the value is not valid.
      * Two parameters can be used in these messages that will be replaced at runtime:
        * {title} : The title of the plugin
        * {value} : The value of the box.
  * The sample project contains a few extra implementations that it needs, e.g. [a question that asks about a uri](https://github.com/StevenThuriot/HyperQube-Plugins/blob/master/Qube.XBMC/Questions/UriQuestion.cs).
  * The `ValidationMessages` class contains a few default messages.

In most cases, you'll want to trigger the input through the tray icon.
You can add menu's to the tray icon for your plugin by implementing `IQubeMenuItem`.

```csharp
public interface IQubeMenuItem
{
  string MenuTitle { get; } //The title shown in the tray.
  void OpenMenu(); //The method that gets called when the menu item is clicked.
}
```

If you only have one menu item, the title will actually be the name of your plugin. The `MenuTitle` is a fallback when your plugin has submenu's. You can give your plugin submenus by implementing `IQubeMenuItemWithSubMenus`, which inherits from `IQubeMenuItem`.

```csharp
public interface IQubeMenuItemWithSubMenus : IQubeMenuItem
{
  IEnumerable<IQubeMenuItem> SubMenuItems { get; }
}
```

This way, you can have as many menu's and submenu's that you need.


After doing all this, simply build your plugin and drop it anywhere in the root or any subfolder of the `HyperQube` executable. `MEF` will automatically pick up your new plugin!
