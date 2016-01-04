ALTER USER 'root'@'localhost' IDENTIFIED BY 'bench';
CREATE SCHEMA `bench` DEFAULT CHARACTER SET utf8;
CREATE TABLE `bench`.`demo` (
  `ID` INT NOT NULL,
  `A` VARCHAR(255) NULL,
  `B` DOUBLE NULL,
  PRIMARY KEY (`ID`));
INSERT INTO `bench`.`demo` (`ID`, `A`, `B`) VALUES ('1', 'Hello World', '1.23');
INSERT INTO `bench`.`demo` (`ID`, `A`, `B`) VALUES ('2', 'Hello Bench', '2.34');
