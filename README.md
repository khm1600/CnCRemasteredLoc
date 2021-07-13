Read the `.loc` files found in one of Command & Conquer Remastered Collection's `.meg` files.

The file format begins with a uint32 specifying the amount of entries, followed by a table of entry infos then entry content, finally entry ids which are strctured as follows:

```cs
// header
UInt32 entryCount;

// entry info
UInt32 unknown; // (The official map editor ignores this value)
UInt32 entryLength; // in characters
UInt32 entryIdLength; // in characters

// entry content
char[] entry; // UTF-16 LE (sometimes "Unicode" in Windows)

// entry ids (i.e. TEXT ID)
char[] id; // ASCII
```

All fields are little-endian
