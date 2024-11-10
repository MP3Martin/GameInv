# GameInv

## Usage

1. Download the [server folder](https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2FMP3Martin%2FGameInv%2Ftree%2Fmain%2Fserver), extract it and open it in your favourite IDE, install the required dependencies and run the program. Or just use any other way to build the project.
2. Run the program and press Y to enter the WS server mode.
3. Open https://mp3martin.github.io/GameInv/ in your browser and everything should connect automatically. Use the cogwheel on the top right to change settings.

## Docs

### Web UI

The Next.js NextUI web interface connects to the [GameInv server](#c-server--console-ui) using a WebSocket connection. It has similar features to the console ui, but with a better [UX](https://en.wikipedia.org/wiki/User_experience_design).

### C# server / console UI

The C# program can either be used as a WebSocket server or in a console with a [TUI](https://en.wikipedia.org/wiki/Text-based_user_interface).

When you start the program, the `Main` method create a new instance of `GameInv`. `GameInv` is the _"main"_ class that holds all the state, such as `IInventory`.

`IInventory` inherits from `IEnumerable<Item>`, which means that you can use it as a `forEach` data source.

Thanks to the program using an interface (`IInventory`) instead of a specific implementation (`Inventory`) of inventory, you can easily crate your own implementation and then change the parameter for `new GameInv` in `Program.cs`.

The items are stored in a **private** list that is located in `Inventory.cs`. They are only modified through the public methods defined in `IInventory`, which ensures encapsulation and ease of implementing a different UI to communicate with the inventory.

When the user performs an action, such as adding a user, the user interface calls `gameInv.Inventory.AddItem`. All of the UI <--> backend communication is done through this way.

The text user interface works thanks to `SimpleMenu` and `MenuPage` abstract classes, which can then be inherited from and the properties like `Title` can get overridden. The nested pages get shown by storing a list of options and then calling `.Show()` on a specific page when an option gets selected.
