/*
Navicat MySQL Data Transfer

Source Server         : Nav
Source Server Version : 80029
Source Host           : localhost:3306
Source Database       : arpgserver

Target Server Type    : MYSQL
Target Server Version : 80029
File Encoding         : 65001

Date: 2022-07-11 17:59:27
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for inventoryitemdb
-- ----------------------------
DROP TABLE IF EXISTS `inventoryitemdb`;
CREATE TABLE `inventoryitemdb` (
  `id` int NOT NULL AUTO_INCREMENT,
  `inventoryid` int DEFAULT NULL,
  `count` int DEFAULT NULL,
  `level` int DEFAULT NULL,
  `isdressed` bit(1) DEFAULT NULL,
  `roleid` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_inventory_roleid` (`roleid`),
  CONSTRAINT `fk_inventory_roleid` FOREIGN KEY (`roleid`) REFERENCES `role` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=70 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of inventoryitemdb
-- ----------------------------
INSERT INTO `inventoryitemdb` VALUES ('49', '1015', '1', '8', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('54', '1013', '1', '6', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('55', '1013', '1', '8', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('56', '1013', '1', '8', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('63', '1002', '1', '2', '', '6');
INSERT INTO `inventoryitemdb` VALUES ('64', '1001', '1', '9', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('65', '1012', '1', '3', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('66', '1018', '1', '5', '\0', '6');
INSERT INTO `inventoryitemdb` VALUES ('67', '1008', '1', '7', '', '6');
INSERT INTO `inventoryitemdb` VALUES ('68', '1001', '1', '6', '\0', '6');

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `level` int DEFAULT NULL,
  `isman` bit(1) DEFAULT NULL,
  `userid` int DEFAULT NULL,
  `exp` int DEFAULT NULL,
  `diamond` int DEFAULT NULL,
  `coin` int DEFAULT NULL,
  `energy` int DEFAULT NULL,
  `toughen` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_userid` (`userid`),
  CONSTRAINT `fk_userid` FOREIGN KEY (`userid`) REFERENCES `user` (`id`) ON DELETE RESTRICT ON UPDATE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES ('6', 'tony', '10', '', '4', '0', '910', '20893', '69', '50');
INSERT INTO `role` VALUES ('8', 'elf2', '1', '', '5', '0', '1000', '20000', '100', '50');
INSERT INTO `role` VALUES ('9', 'elf3', '1', '', '6', '0', '1000', '20000', '100', '50');

-- ----------------------------
-- Table structure for serverproperty
-- ----------------------------
DROP TABLE IF EXISTS `serverproperty`;
CREATE TABLE `serverproperty` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `ip` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `count` int DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=25 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of serverproperty
-- ----------------------------
INSERT INTO `serverproperty` VALUES ('1', '一区 小花果山', '127.0.0.1', '10');
INSERT INTO `serverproperty` VALUES ('2', '二区 美猴王', '127.0.0.1', '20');
INSERT INTO `serverproperty` VALUES ('3', '三区 长沙', '127.0.0.1', '30');
INSERT INTO `serverproperty` VALUES ('4', '四区 哈哈哈', '127.0.0.1', '40');
INSERT INTO `serverproperty` VALUES ('5', '一区 花果山', '127.0.0.1', '50');
INSERT INTO `serverproperty` VALUES ('6', '二区 美猴王', '127.0.0.1', '60');
INSERT INTO `serverproperty` VALUES ('7', '三区 长沙', '127.0.0.1', '200');
INSERT INTO `serverproperty` VALUES ('8', '四区 哈哈哈', '127.0.0.1', '150');
INSERT INTO `serverproperty` VALUES ('9', '五区 测试服', '127.0.0.1', '30');
INSERT INTO `serverproperty` VALUES ('10', '六区 撒谎', '127.0.0.1', '40');
INSERT INTO `serverproperty` VALUES ('11', '七区 非付费', '127.0.0.1', '300');
INSERT INTO `serverproperty` VALUES ('12', '八区 发表', '127.0.0.1', '60');
INSERT INTO `serverproperty` VALUES ('13', '九区 家教', '127.0.0.1', '10');
INSERT INTO `serverproperty` VALUES ('14', '一区 花果山', '127.0.0.1', '100');
INSERT INTO `serverproperty` VALUES ('15', '二区 美猴王', '127.0.0.1', '30');
INSERT INTO `serverproperty` VALUES ('16', '三区 长沙', '127.0.0.1', '40');
INSERT INTO `serverproperty` VALUES ('17', '四区 哈哈哈', '127.0.0.1', '50');
INSERT INTO `serverproperty` VALUES ('18', '五区 测试服', '127.0.0.1', '60');
INSERT INTO `serverproperty` VALUES ('19', '六区 撒谎', '127.0.0.1', '10');
INSERT INTO `serverproperty` VALUES ('20', '七区 非付费', '127.0.0.1', '20');
INSERT INTO `serverproperty` VALUES ('21', '八区 发表', '127.0.0.1', '100');
INSERT INTO `serverproperty` VALUES ('22', '九区 家教', '127.0.0.1', '40');
INSERT INTO `serverproperty` VALUES ('23', '一区 花果山', '127.0.0.1', '50');
INSERT INTO `serverproperty` VALUES ('24', '二区 美猴王', '127.0.0.1', '60');

-- ----------------------------
-- Table structure for skilldb
-- ----------------------------
DROP TABLE IF EXISTS `skilldb`;
CREATE TABLE `skilldb` (
  `id` int NOT NULL AUTO_INCREMENT,
  `skillid` int DEFAULT NULL,
  `roleid` int DEFAULT NULL,
  `level` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_skillid_roleid` (`roleid`),
  CONSTRAINT `fk_skillid_roleid` FOREIGN KEY (`roleid`) REFERENCES `role` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of skilldb
-- ----------------------------
INSERT INTO `skilldb` VALUES ('1', '1002', '6', '3');
INSERT INTO `skilldb` VALUES ('2', '1003', '6', '2');
INSERT INTO `skilldb` VALUES ('3', '1004', '6', '3');

-- ----------------------------
-- Table structure for taskdb
-- ----------------------------
DROP TABLE IF EXISTS `taskdb`;
CREATE TABLE `taskdb` (
  `id` int NOT NULL AUTO_INCREMENT,
  `taskid` int DEFAULT NULL,
  `state` tinyint DEFAULT NULL,
  `type` tinyint DEFAULT NULL,
  `roleid` int DEFAULT NULL,
  `lastupdatetime` datetime DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `fk_roleid` (`roleid`),
  CONSTRAINT `fk_roleid` FOREIGN KEY (`roleid`) REFERENCES `role` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of taskdb
-- ----------------------------
INSERT INTO `taskdb` VALUES ('4', '1001', '2', '0', '6', '0001-01-01 00:00:00');
INSERT INTO `taskdb` VALUES ('5', '1002', '1', '1', '6', '0001-01-01 00:00:00');
INSERT INTO `taskdb` VALUES ('6', '1003', '1', '2', '6', '0001-01-01 00:00:00');
INSERT INTO `taskdb` VALUES ('7', '1003', '1', '2', '9', '0001-01-01 00:00:00');
INSERT INTO `taskdb` VALUES ('8', '1003', '1', '2', '8', '0001-01-01 00:00:00');
INSERT INTO `taskdb` VALUES ('9', '1002', '1', '1', '8', '0001-01-01 00:00:00');
INSERT INTO `taskdb` VALUES ('10', '1002', '1', '1', '9', '0001-01-01 00:00:00');

-- ----------------------------
-- Table structure for user
-- ----------------------------
DROP TABLE IF EXISTS `user`;
CREATE TABLE `user` (
  `id` int NOT NULL AUTO_INCREMENT,
  `username` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  `password` varchar(255) COLLATE utf8mb4_general_ci DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=7 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- ----------------------------
-- Records of user
-- ----------------------------
INSERT INTO `user` VALUES ('2', 'elll', '81DC9BDB52D04DC20036DBD8313ED055');
INSERT INTO `user` VALUES ('3', 'elff', '674F3C2C1A8A6F90461E8A66FB5550BA');
INSERT INTO `user` VALUES ('4', 'elf', '202CB962AC59075B964B07152D234B70');
INSERT INTO `user` VALUES ('5', 'elf2', '202CB962AC59075B964B07152D234B70');
INSERT INTO `user` VALUES ('6', 'elf3', '202CB962AC59075B964B07152D234B70');
