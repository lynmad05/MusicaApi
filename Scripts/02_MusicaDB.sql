DROP TABLE IF EXISTS Cancion;
DROP TABLE IF EXISTS Album;

CREATE TABLE Album (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(100) NOT NULL,
    Artista NVARCHAR(100) NOT NULL,
    Genero NVARCHAR(50) NOT NULL,
    AnioLanzamiento INT NOT NULL,
    CONSTRAINT UQ_Album_Titulo_Artista UNIQUE (Titulo, Artista),
    CONSTRAINT CK_Album_Anio CHECK (AnioLanzamiento BETWEEN 1900 AND 2100)
);

CREATE TABLE Cancion (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    AlbumId INT NOT NULL,
    Titulo NVARCHAR(100) NOT NULL,
    NumeroPista INT NOT NULL,
    DuracionSegundos INT NOT NULL,
    Reproducciones INT NOT NULL DEFAULT 0,
    CONSTRAINT FK_Cancion_Album FOREIGN KEY (AlbumId) REFERENCES Album(Id),
    CONSTRAINT UQ_Cancion_Album_Pista UNIQUE (AlbumId, NumeroPista),
    CONSTRAINT CK_Cancion_Duracion CHECK (DuracionSegundos BETWEEN 1 AND 7200)
);

INSERT INTO Album (Titulo, Artista, Genero, AnioLanzamiento) VALUES
(N'Abbey Road', N'The Beatles', N'Rock', 1969),
(N'Thriller', N'Michael Jackson', N'Pop', 1982),
(N'Un Verano Sin Ti', N'Bad Bunny', N'Reggaetón', 2022),
(N'Kind of Blue', N'Miles Davis', N'Jazz', 1959);

INSERT INTO Cancion (AlbumId, Titulo, NumeroPista, DuracionSegundos, Reproducciones) VALUES
(1, N'Come Together', 1, 259, 1200),
(1, N'Something', 2, 183, 800),
(1, N'Maxwell''s Silver Hammer', 3, 207, 350),
(1, N'Oh! Darling', 4, 206, 410),
(2, N'Wanna Be Startin'' Somethin''', 1, 363, 900),
(2, N'Thriller', 4, 357, 2500),
(2, N'Beat It', 5, 258, 2100),
(2, N'Billie Jean', 6, 294, 3000),
(3, N'Moscow Mule', 1, 245, 1800),
(3, N'Me Porto Bonito', 3, 178, 2600),
(3, N'Tití Me Preguntó', 4, 243, 2400),
(4, N'So What', 1, 562, 700),
(4, N'Freddie Freeloader', 2, 589, 520),
(4, N'Blue in Green', 3, 337, 640),
(4, N'All Blues', 4, 696, 480);