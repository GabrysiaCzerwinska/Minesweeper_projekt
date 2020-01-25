# Minesweeper [wersja alpha]

## Podzia� pracy

* Gabriela Czerwi�ska
    * projekt graficzny
    * u�o�enie element�w w pliku `MainWindow.xaml`
    * klasa `Field` (atrybuty, konstruktor, metody) w pliku `Field.cs`
    * metoda `drawBoard` w pliku `Board.cs`
* Aleksander Czubat
    * klasa `Board` (atrybuty, konstruktor) w pliku `Board.cs`
    * metoda `createBoard` w pliku `Board.cs`
    * stoper (metody: `OnTimedEvent`, `resetGame`) w pliku `MainWindow.xaml.cs`
* Kamil Chachurski
    * metody: `exposeHiddenFields`, `exposeAllFields`, `handleLeftMouseButtonClick` i `handleRightMouseButtonClick` w pliku `Board.cs`
    * obs�uga przycisk�w startu (metody: `clickBeginnerButton`, `clickIntermediateButton`, `clickExpertButton`) w pliku `MainWindow.xaml.cs`
    * dokumentacja
* Jakub Dedio:
    * aplikacja [wersja beta] - stworzenie projektu wed�ug nakazanych kryteri�w, opieraj�c si� na logice i funkcjonalno�ciach z wersji alpha

## Klasy

### Field

Klasa opisuje pole na planszy do gry. Obiekt typu `Field` trzyma informacje o swoim stanie, na przyk�ad czy jest zaminowane.

#### Metody

* `increaseDangerLevel` edytuje atrybut `dangerLevel`
* `mine` edytuje atrybut `isMined`
* `changeSuspect` edytuje atrybut `isSuspected`
* `expose` edytuje atrybut `isExposed`

### Board

Klasa jest odpowiedzialna za stworzenie planszy i narysowanie (wykorzystuj�c do tego klas� `Field`), obs�ug� klikni�cie i od�wie�anie widoku.

#### Metody

* `createBoard` wype�nia tablic� `board` obiektami typu `Field` oraz w losowych miejscach "zaminowywuje pola"
* `drawBoard` na podstawie tablicy `board` rysuje plansz� w oknie aplikacji, zwracaj�c uwag� na stan pola
* `exposeHiddenFields` pod okre�lonymi warunkami ods�ania pola wok� klikni�tego pola
* `exposeAllFields` ods�ania wszystkie pola
* `handleLeftMouseButtonClick` obs�uguje klikni�cie lewym przyciskiem myszy na plansz� - ods�oni�cie pola
* `handleRightMouseButtonClick` obs�uguje klikni�cie prawym przyciskiem myszy na plansz� - ustawienie pola na podejrzane

### MainWindow

Klasa jest sercem aplikacji. Zleca ona stworzenie planszy, obs�uguje timer i klikni�cia przycisk�w kontroluj�cych aplikacje.

#### Metody

* `OnTimedEvent` obs�uguje cykl stopera
* `resetGame` resetuje ustawienia aktualnej gry
* `clickBeginnerButton` rozpoczyna gr� na �atwym poziomie trudno�ci
* `clickIntermediateButton` rozpoczyna gr� na normalnym poziomie trudno�ci
* `clickExpertButton` rozpoczyna gr� na trudnym poziomie trudno�ci