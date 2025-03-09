-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema gameinv
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema gameinv
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `gameinv` DEFAULT CHARACTER SET utf8 ;
USE `gameinv` ;

-- -----------------------------------------------------
-- Table `gameinv`.`items`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `gameinv`.`items` (
  `id` CHAR(36) NOT NULL,
  `sortOrder` BIGINT UNSIGNED NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(70) NOT NULL,
  `damagePerTick` SMALLINT UNSIGNED NULL DEFAULT NULL,
  `damagePerUse` SMALLINT UNSIGNED NULL DEFAULT NULL,
  `durability` SMALLINT UNSIGNED NULL DEFAULT NULL,
  UNIQUE INDEX `id_UNIQUE` (`id` ASC) VISIBLE,
  PRIMARY KEY (`id`),
  UNIQUE INDEX `sortOrder_UNIQUE` (`sortOrder` ASC) VISIBLE)
ENGINE = InnoDB;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
