--
-- PostgreSQL database dump
--

-- Dumped from database version 10.20
-- Dumped by pg_dump version 10.20

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- Name: plpgsql; Type: EXTENSION; Schema: -; Owner: 
--

CREATE EXTENSION IF NOT EXISTS plpgsql WITH SCHEMA pg_catalog;


--
-- Name: EXTENSION plpgsql; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION plpgsql IS 'PL/pgSQL procedural language';


SET default_tablespace = '';

SET default_with_oids = false;

--
-- Name: tour_logs; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tour_logs (
    tour_id integer,
    date_time timestamp without time zone,
    tour_comment character varying,
    difficulty integer,
    total_time integer,
    rating integer,
    id integer NOT NULL
);


ALTER TABLE public.tour_logs OWNER TO postgres;

--
-- Name: tour_logs_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tour_logs_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tour_logs_id_seq OWNER TO postgres;

--
-- Name: tour_logs_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tour_logs_id_seq OWNED BY public.tour_logs.id;


--
-- Name: tours; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tours (
    id integer NOT NULL,
    name character varying,
    description character varying,
    tour_from character varying,
    transport_type character varying,
    distance numeric,
    "time" integer,
    route_information character varying,
    tour_to character varying
);


ALTER TABLE public.tours OWNER TO postgres;

--
-- Name: tours_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tours_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER TABLE public.tours_id_seq OWNER TO postgres;

--
-- Name: tours_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tours_id_seq OWNED BY public.tours.id;


--
-- Name: tour_logs id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tour_logs ALTER COLUMN id SET DEFAULT nextval('public.tour_logs_id_seq'::regclass);


--
-- Name: tours id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tours ALTER COLUMN id SET DEFAULT nextval('public.tours_id_seq'::regclass);


--
-- Data for Name: tour_logs; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tour_logs (tour_id, date_time, tour_comment, difficulty, total_time, rating, id) FROM stdin;
1	2022-06-09 18:29:27.255581	asdasd	3	4	4	1
7	2022-06-15 14:59:15.2053	asd	4	4	4	2
7	2022-06-15 15:00:54.427477	asd	4	4	4	3
7	2022-06-15 15:01:22.263414	asd	4	4	4	4
\.


--
-- Data for Name: tours; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tours (id, name, description, tour_from, transport_type, distance, "time", route_information, tour_to) FROM stdin;
1	Test1	Some Desc 1	Test	Test	10	10	https:test	errr
2	Test1	Some Desc 1	Test	Test	10	10	https:test	errr
5	Test1	Some Desc 1	Test	Test	10	10	https:test	Test
7	Test1	Some Desc 1	Test	Test	10	10	https:test	Test
6	asd	Asd	Test	Test	10	10	https:test	Test
9	Test1	Some Desc 1	Test	Test	10	10	https:test	Test
10	Test1	Some Desc 1	Test	Test	10	10	https:test	Test
11	Test1	Some Desc 1	Test	Test	10	10	asddddddddddd	Test
\.


--
-- Name: tour_logs_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tour_logs_id_seq', 4, true);


--
-- Name: tours_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tours_id_seq', 11, true);


--
-- Name: tour_logs tour_logs_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tour_logs
    ADD CONSTRAINT tour_logs_pkey PRIMARY KEY (id);


--
-- Name: tours tours_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tours
    ADD CONSTRAINT tours_pkey PRIMARY KEY (id);


--
-- Name: tour_logs myconstraint; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tour_logs
    ADD CONSTRAINT myconstraint FOREIGN KEY (tour_id) REFERENCES public.tours(id);


--
-- PostgreSQL database dump complete
--

