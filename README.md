A simple algorithm build in C# that uilises dictionaries to compress text files.

**Usage**<br>
The main function CompressTextFile() takes a file path as a parameter, and an unsigned 16 bit integer for the number of words in a sequence (which is then mapped to an integer key in the dictionary)

Example code:
<pre>
  CompressTextFile("data.txt", 3);
</pre>

Output:
<pre>
  Compressed data to data.lzw
  
  Original file size 158 KB
  
  Compressed file size 52 KB
  
  32.91% reduction in file size
</pre>
