# AssetBundleCacher
A Unity tool that downloads an assetbundle and caches it on the local machine.
Cached assetbundles use a different compresssion than usual editor built ones, and recompressing existing bundles is difficult without using Unity to do it.
This tool usues the caching system of unity to achieve this recompression.
Files are saved to a folder based on an ID and a version provided via command line parameters.
The cache path and file compression can be optionally overridden.
By default the cache path is in AppData\LocalLow\UnityTools\AssetBundleCacher\cache, and bundles are saved with LZ4 Runtime compression.

Usage:
 -url            URL to download
 -id             ID of the file
 -ver            file version
 -path           cache path override
 -uncompressed   save uncompressed bundles
