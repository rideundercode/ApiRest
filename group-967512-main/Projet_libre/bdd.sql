CREATE TABLE address(
id              INT             PRIMARY KEY,
user_id        INT             NOT NULL,
street          VARCHAR(200)    NOT NULL,
city            VARCHAR(200)    NOT NULL,
postal_code            VARCHAR(200)    NOT NULL,
country         VARCHAR(200)    NOT NULL,
creation_date    DATETIME,
update_date      DATETIME,
FOREIGN KEY (user_id) REFERENCES user(id)
);

CREATE TABLE realisateur(
id              INT             PRIMARY KEY,
nom_complet     VARCHAR(200)    NOT NULL,
nationalite     VARCHAR(200),
photo           VARCHAR(200),
biographie      VARCHAR(200),
naissance       DATETIME,
creation_date   DATETIME,
update_date     DATETIME
);

CREATE TABLE user(
id              INT     PRIMARY KEY,
address_id      INT     NOT NULL,
username        VARCHAR(200)    NOT NULL,
password        VARCHAR(200)    NOT NULL,
firstname       VARCHAR(200)    NOT NULL,
lastname        VARCHAR(200)    NOT NULL,
role        VARCHAR(200)    NOT NULL,
creation_date   DATETIME,
update_date     DATETIME,
FOREIGN KEY (address_id) REFERENCES address(id)
);

CREATE TABLE article(
id              INT             PRIMARY KEY,
realisateur_id  INT             NOT NULL,
titre           VARCHAR(200)    NOT NULL,
duree           INT,
categorie       VARCHAR(200),
type_article    VARCHAR(200),
date_sortie     DATETIME,
creation_date   DATETIME,
updated_date    DATETIME,
FOREIGN KEY (realisateur_id) REFERENCES realisateur(id)
);

CREATE TABLE comment(
id              INT             PRIMARY KEY,
user_id         INT             NOT NULL,
article_id              INT             NOT NULL,
contenu         VARCHAR(200),
creation_date   DATETIME,
updated_date    DATETIME,
FOREIGN KEY (user_id) REFERENCES user(id),
FOREIGN KEY (article_id) REFERENCES article(id)
);

CREATE TABLE commande(
id              INT             PRIMARY KEY,
user_id         INT             NOT NULL,
article_id      INT             NOT NULL,
creation_date   DATETIME,
FOREIGN KEY (user_id) REFERENCES user(id),
FOREIGN KEY (article_id) REFERENCES article(id)
);

