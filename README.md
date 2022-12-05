# Wordle
An assignment based on **Wordle**, made for Futuregames course "Data Structures and Algorithms".

Before you read on, get a hands on feel for the project over at [itch.io](https://joebinns.itch.io/wordle).

## Controls
Write out letters using your <kbd>Keyboard</kbd>.  
Enter guesses using <kbd>Enter</kbd>.  
Undo letters using <kbd>Backspace</kbd>.  
Alternatively, use the on-screen keyboard with your <kbd>Mouse</kbd>.

## Write-up
Searching through a list of 16,000 words every time I want to check if a word is valid or not is extremely inefficient. Since each words in the list is unique, I decided to instead use a hash set to store the words. Now, only a single hash function needs to be calculated to check whether or not a word is valid.

I decided to use Tilemaps to draw the guesses text. In order to uphold abstraction, I modified and stored the text through a custom text editor script. The text editor script stored the entire text in a single list. I then created convenient accessors, such as for fetching the word stored on the current line.

## Credits
### Sound Effects
The sound effects and audio were kindly created and arranged by [Clara Summerton](mailto:clarasummerton@gmail.com).

### All else
Is [my own](https://joebinns.com/).

## Development Log
### Week 2
This week I've been adding animations to a 'Wordle' assignment I've been working on at Futuregames. The animations are purely code based, and run on a modular queue-based animation system. This simple solution works well for slow paced gameplay!

[<img alt="Wordle Clone: Animations!" width="503" src="https://joebinns.com/documents/gifs/wordle.gif" />](https://youtu.be/C2FEdz-75C8)
