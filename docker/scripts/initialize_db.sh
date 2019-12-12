psql postgres  <<EOSQL
	CREATE DATABASE rocketlunch;

	CREATE USER admin WITH ENCRYPTED PASSWORD 'admin';
	GRANT ALL PRIVILEGES ON DATABASE rocketlunch TO admin;
EOSQL

# Schema
psql rocketlunch <<EOSQL

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET client_min_messages = warning;
SET row_security = off;
SET default_tablespace = '';
SET default_with_oids = false;


CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);
ALTER TABLE public."__EFMigrationsHistory" OWNER TO admin;
ALTER TABLE ONLY public."__EFMigrationsHistory" ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


CREATE TABLE public.users
(
    "id" integer NOT NULL,
    "name" text,
    "nopes" text,
    "google_id" text,
    "email" text,
    "photo_url" text
);
ALTER TABLE public.users OWNER to admin;
ALTER TABLE ONLY public.users ADD CONSTRAINT "pk_users" PRIMARY KEY ("id");

CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;

ALTER TABLE public.users_id_seq OWNER TO admin;
ALTER TABLE ONLY public.users ALTER COLUMN id SET DEFAULT nextval('public.users_id_seq');

EOSQL

# Data
psql rocketlunch <<EOSQL

INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20180830023654_CreateUserTable', '2.1.2-rtm-30932');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20180830220719_Nooooos', '2.1.2-rtm-30932');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20191107150904_AddingGoogleIdAndEmail', '3.0.0');
INSERT INTO public."__EFMigrationsHistory" ("MigrationId", "ProductVersion") VALUES ('20191108171755_PhotoUrl', '3.0.0');

INSERT INTO public.users ("name", "google_id", "email", "nopes", "photo_url") VALUES ('Oxford Labs', '111246303330212276863', 'oxfordlabsclgx@gmail.com', '["CntO5_U6B1kOlyfjZDuxFA"]', 'https://lh5.googleusercontent.com/-jRxBy2YsKjI/AAAAAAAAAAI/AAAAAAAAAAA/ACHi3rdznSLQRIvj2YwmG2LgxqPJfGJgOw/s96-c/photo.jpg');
INSERT INTO public.users ("name", "google_id", "email", "nopes", "photo_url") VALUES ('Hamburglar', 'abc123', 'patty@sizzle.greese', '["CntO5_U6B1kOlyfjZDuxFA"]', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSz-ysAsOu__CmLz6l88jKrtnL55SrWLgTUSUxyaUdbZZkfPsmi&s');
INSERT INTO public.users ("name", "google_id", "email", "nopes", "photo_url") VALUES ('Cookie Monster', 'pilsbury', 'chips@ahoy.dough', '["XatxqH9k2auTRSKzGKBL0w"]', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQMfFMZFpKVx0oE9wvWdvs5wHr33pMGzVjkrYQggDy-3uVcxz8cOg&s');
INSERT INTO public.users ("name", "google_id", "email", "nopes", "photo_url") VALUES ('Swedish Chef', 'sweeeeeeeeed', 'meatballs@healthcare.knives', '["iuS0buU4WW3j_Lk4UCOcQA"]', 'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT9qlbS_wjk1Nf6jOHvNZ9zcV5_-Bv2EOqoWjFGpymmH0k097Ow&s');

EOSQL