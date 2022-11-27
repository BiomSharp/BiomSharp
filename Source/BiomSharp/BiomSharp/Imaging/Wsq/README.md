![Logo](https://github.com/BiomSharp/BiomSharp/blob/master/.branding/logo/github_logo.png)
######

## WSQ-encoded fingerprint images

This image codec is a pure-C# implementation of the <a href="https://www.nist.gov/itl/iad/image-group/wsq-bibliography" target="_blank">"Wavelet Scalar Quantization (WSQ) Gray-Scale Fingerprint Image Compression Specification Version 3.1"</a>. It provides for encoding and decoding of WSQ images. This image format is the most common industry-standard format used for the exchange of fingerprints. It supports only 8-bit gray-scale images, ideally with resolutions of 500 ppi (pixels-per-inch).

## WSQ Certification

This C# implementation has passed the NIST <a href="https://www.nist.gov/programs-projects/wsq-certification-procedure" target="_blank">WSQ Certification Process</a>.

We own the following implementations:

Organization: <a href="https://bzw.co.za" target="_blank">Businessware Architects</a> algorithm 11340/11341 as documented here: <a href="https://fbibiospecs.fbi.gov/certifications-1/wsq" target="_blank">WSQ Fingerprint Image Compression Encoder/Decoder Certification</a>

## Provisions of this implementation

**Please note:**
1. The provided C# implementation is subject to the <a href="https://github.com/BiomSharp/BiomSharp/blob/master/LICENSE.txt" target="_blank">MIT</a> license.
1. The vendor number is set to '0', which is not allocated to any organization. Please **DO NOT** change this value if the source code is incorporated in your own software.

## Features
1. Based on the <a href="https://www.nist.gov/services-resources/software/nist-biometric-image-software-nbis" target="_blank">NIST NBIS</a> WSQ codec implementation, it allows reading and writing of the NIST-standard image information headers, and comment blocks in the encoded WSQ image.
2. Uses a proper object-oriented approach, allows for the easy customization of the format by the addition of user-defined tags/blocks.
3. Provides the following settings as codec parameters:
    * Placing the Huffman Tables (DHT) into a single or multiple discrete blocks.
    * Specifying the WSQ filter taps as the recommended 7x9, or 8x8 kernels.
    * Adjustment of the WSQ compression bit-rates.


## Documentation

To follow.
