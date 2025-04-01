[![CodeFactor](https://www.codefactor.io/repository/github/MP3Martin/GameInv/badge)](#/)
[<img src="https://img.shields.io/github/license/MP3Martin/GameInv">](#/)
[<img src="https://img.shields.io/github/stars/MP3Martin/GameInv?style=flat">](#/)
[<img src="https://img.shields.io/github/forks/MP3Martin/GameInv?style=flat">](#/)
[<img src="https://img.shields.io/github/issues/MP3Martin/GameInv">](#/)

# GameInv

<img src="https://github.com/user-attachments/assets/ec6888ef-99f1-45ac-98d6-dd92d97555fa" width="950px" />

## Usage

1. Get the GameInv server
    1. by downloading the [latest build from releases](https://github.com/MP3Martin/GameInv/releases/latest/)
    2. <details><summary>or ...</summary>or download the <a href="https://download-directory.github.io/?url=https%3A%2F%2Fgithub.com%2FMP3Martin%2FGameInv%2Ftree%2Fmain%2Fserver">server folder</a>, extract it and open it in your favourite IDE, install the required dependencies, modify the code however you want and run the program. Or just use any other way to build the project.</details>
2. Run the program (the server)
3. Press Y to enter the WS server mode.
4. Press Y if you want to use [MySQL DB](#mysql). (has to be configured first, otherwise select N)
5. Open https://mp3martin.github.io/GameInv/ in your browser and everything should connect automatically. Use the
   cogwheel on the top right to change settings.

> [!WARNING]  
> Do not run the server publicly, it is just a demo/showcase and not advanced enough to be secure and handle attacks.

> [!IMPORTANT]  
> Microsoft Defender sometimes reports the automatically built exes as a virus. The current fix is to allow the `.exe`
> in Defender.

## Configuration

### `.env`

This program allows you to configure various options through the `.env` file or by setting environment variables. The
`.env` file has to be placed in the same directory as the program executable. The [`.env.example`](server/.env.example)
file provides a description for these settings, so read it. To use it, rename `.env.example` to `.env` and modify the
values as needed. Note that environment variables set in the system or command line must be prefixed with `GAMEINV_`,
but the `.env` file does not require this prefix.

### MySQL

If you want the items to be stored in a database (MySQL is currently the only supported one), then you have to connect
to / start a MySQL server and run the commands inside [gameinv.sql](server/gameinv.sql) to add the schema and the
tables (find a tutorial elsewhere). Then [configure](#configuration) your program to use the MySQL DB.

## GUI

There is also a WPF GUI version of the project in [server/GameInv-WPF](server/GameInv-WPF). It only supports Windows 7+
and no
other OS. Tested on Windows 11. That version does not have a WebSocket client, does not have TUI and only has an
integrated WPF GUI with optional DB connection. The usage is the same as in [usage](#usage), but skip steps 3 and 5. If
you want to download a prebuilt `.exe`, then find the one that has `WPF` in its
name [in the latest release](https://github.com/MP3Martin/GameInv/releases/latest/).

## More docs

Can be found [here](DOCS.md)
