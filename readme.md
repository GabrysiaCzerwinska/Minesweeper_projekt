# Minesweeper [wersja alpha]

## Podzia³ pracy

* Gabriela Czerwiñska
    * projekt graficzny
    * u³o¿enie elementów w pliku `MainWindow.xaml`
    * klasa `Field` (atrybuty, konstruktor, metody) w pliku `Field.cs`
    * metoda `drawBoard` w pliku `Board.cs`
* Aleksander Czubat
    * klasa `Board` (atrybuty, konstruktor) w pliku `Board.cs`
    * metoda `createBoard` w pliku `Board.cs`
    * stoper (metody: `OnTimedEvent`, `resetGame`) w pliku `MainWindow.xaml.cs`
* Kamil Chachurski
    * metody: `exposeHiddenFields`, `exposeAllFields`, `handleLeftMouseButtonClick` i `handleRightMouseButtonClick` w pliku `Board.cs`
    * obs³uga przycisków startu (metody: `clickBeginnerButton`, `clickIntermediateButton`, `clickExpertButton`) w pliku `MainWindow.xaml.cs`
    * dokumentacja
* Jakub Dedio:
    * aplikacja [wersja beta] - stworzenie projektu wed³ug nakazanych kryteriów, opieraj¹c siê na logice i funkcjonalnoœciach z wersji alpha

## Klasy

### Field

Klasa opisuje pole na planszy do gry. Obiekt typu `Field` trzyma informacje o swoim stanie, na przyk³ad czy jest zaminowane.

#### Metody

* `increaseDangerLevel` edytuje atrybut `dangerLevel`
* `mine` edytuje atrybut `isMined`
* `changeSuspect` edytuje atrybut `isSuspected`
* `expose` edytuje atrybut `isExposed`

### Board

Klasa jest odpowiedzialna za stworzenie planszy i narysowanie (wykorzystuj¹c do tego klasê `Field`), obs³ugê klikniêcie i odœwie¿anie widoku.

#### Metody

* `createBoard` wype³nia tablicê `board` obiektami typu `Field` oraz w losowych miejscach "zaminowywuje pola"
* `drawBoard` na podstawie tablicy `board` rysuje planszê w oknie aplikacji, zwracaj¹c uwagê na stan pola
* `exposeHiddenFields` pod okreœlonymi warunkami ods³ania pola wokó³ klikniêtego pola
* `exposeAllFields` ods³ania wszystkie pola
* `handleLeftMouseButtonClick` obs³uguje klikniêcie lewym przyciskiem myszy na planszê - ods³oniêcie pola
* `handleRightMouseButtonClick` obs³uguje klikniêcie prawym przyciskiem myszy na planszê - ustawienie pola na podejrzane

### MainWindow

Klasa jest sercem aplikacji. Zleca ona stworzenie planszy, obs³uguje timer i klikniêcia przycisków kontroluj¹cych aplikacje.

#### Metody

* `OnTimedEvent` obs³uguje cykl stopera
* `resetGame` resetuje ustawienia aktualnej gry
* `clickBeginnerButton` rozpoczyna grê na ³atwym poziomie trudnoœci
* `clickIntermediateButton` rozpoczyna grê na normalnym poziomie trudnoœci
* `clickExpertButton` rozpoczyna grê na trudnym poziomie trudnoœci