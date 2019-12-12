CREATE DATABASE rocketlunch;
USE rocketlunch;

CREATE TABLE __EFMigrationsHistory (
    MigrationId VARCHAR(150) NOT NULL,
    ProductVersion VARCHAR(32) NOT NULL
);
ALTER TABLE __EFMigrationsHistory ADD CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId);

CREATE TABLE IF NOT EXISTS users (
  id INT(11) NOT NULL AUTO_INCREMENT,
  google_id TEXT NULL DEFAULT NULL,
  name TEXT NULL DEFAULT NULL,
  email TEXT NULL DEFAULT NULL,
  photo_url TEXT NULL DEFAULT NULL,
  nopes TEXT NULL DEFAULT NULL,
  PRIMARY KEY (id));

INSERT INTO __EFMigrationsHistory (MigrationId, ProductVersion) VALUES ('20191212173057_init', '2.2.4-servicing-10062');

INSERT INTO users (name, google_id, email, nopes) VALUES ('Oxford Labs', '111246303330212276863', 'oxfordlabsclgx@gmail.com', '[''CntO5_U6B1kOlyfjZDuxFA'']');
INSERT INTO users (name, google_id, email, nopes) VALUES ('Hamburglar', 'abc123', 'patty@sizzle.greese', '[''CntO5_U6B1kOlyfjZDuxFA'']');
INSERT INTO users (name, google_id, email, nopes) VALUES ('Cookie Monster', 'pilsbury', 'chips@ahoy.dough', '[''XatxqH9k2auTRSKzGKBL0w'']');
INSERT INTO users (name, google_id, email, nopes) VALUES ('Swedish Chef', 'sweeeeeeeeed', 'meatballs@healthcare.knives', '[''iuS0buU4WW3j_Lk4UCOcQA'']');
