package xyz.mp3martin.gameInvSpigotClient

import org.bukkit.plugin.java.JavaPlugin

class GameInvSpigotClient : JavaPlugin() {

    override fun onEnable() {
        // Plugin startup logic
        logger.info("GameInv spigot client is working!")
    }

    override fun onDisable() {
        // Plugin shutdown logic
    }
}
