![Logo](https://github.com/BiomSharp/BiomSharp/blob/master/.branding/logo/github_logo.png)
######
BiomSharp, or 'Biom#', is a set of C# libraries and demo applications that provide easier and more consistent management of person biometric data, as well as an abstraction framework for working with various biometric sensors. Some of the biometric functions are provided as C# class libraries that interoperate with C/C++ native libraries.

## Motivation

Faced with client requirements to provide fingerprint verification and identification of persons, while maintaining a high degree of vendor independence as regards scanning devices and SDK's, we have created a C# framework to abstract the processes and data of biometric capture. The framework is written with the intent to provide multiple biometric modalities, although it is currently developed for fingerprint and hand-palm capture.

Our full framework also provides functionality required for biometric analysis separate from vendor SDK's, including minutiae detection, extraction, and print quality measures, mainly by interfacing to existing open-source libraries for biometric functions, image-format conversion, image-processing and other utilities. The framework provides access to biometric scanners functions via a C# 'wrapper' of the corresponding functions in the SDK, the functionality abstracted and exposed through a set of standard interfaces defined in the framework core library. The ultimate goal in all instances of such wrappers is to use the underlying SDK only for print-capture, while the rest of the process is present in the framework itself. In cases where C# interoperability with underlying C/C++ libraries are required, this framework endeavours to provide such functionality 'in-the-box', or a rewrite in pure C#.

## Why C#?

Most of our clients require deployments on the Microsoft Windows operating system. Also, as most biometric vendors provide mainly C++ and nowadays, C# SDK's, it has become our path of least-resistance. It is our experience that using a modern memory-managed framework/language leads to higher quality solutions and faster delivery times. Add to this the fact that most application developers today are less experience in C/C++, makes this type of framework a more attractive alternative.

Secondly, .NET (and C#) is gaining wider acceptance as a credible cross-platform development environment on desktop, mobile and back-end systems and applications. The wider appeal is being seen as a greater demand for C#/.NET to be used for developing end-to-end biometric-based solutions.

## Status

This Github published framework is currently a (very) incomplete version of the full framework. The idea is to release more functionality on an ongoing basis.

## Cross-platform portability

We endeavour to provide the code as 'platform-neutral' as possible - most of the projects are portable to any of the existing .NET 6.0 target platforms. Also, they do not reference Windows-specific packages such as 'System.Drawing.Common' or the Windows application frameworks, Windows-Forms, WPF/UWP, etc. Where they are explicitly referenced they are so documented.

## Requirements

The framework currently supports .NET 6.0 core. The applications and libraries are provided as a set of Visual Studio solutions (.sln), and C# projects (.csproj) or C++ projects (.vcxproj). The existing builds are for Microsoft Windows and target x64 processor architectures.

We recommend using Visual Studio 2022 (any edition).

## License

[MIT](https://github.com/BiomSharp/BiomSharp/blob/master/LICENSE.txt)

## Version

Um - not sure we're ready for versioning yet.

## Current features

1. C# implementation of the <a href="https://github.com/BiomSharp/BiomSharp/tree/master/Source/BiomSharp/BiomSharp/Imaging/Wsq#readme" target="_blank">WSQ codec</a>. This codec is NIST/FBI certified.
1. C#-wrapper implementation of the <a href="https://www.nist.gov/services-resources/software/nfiq-2" target="_blank">NIST NFIQ 2</a> (version 2.2) fingerprint quality measure.
1. Integration to <a href="https://github.com/BiomSharp/BiomSharp/blob/master/Source/BiomSharp/BiomSharp.Windows#readme" target="_blank">Windows-codecs</a> and <a href="https://github.com/BiomSharp/BiomSharp/blob/master/Source/BiomSharp/BiomSharp.ImageSharp#readme" target="_blank">ImageSharp-codecs</a>.
1. <a href="https://github.com/BiomSharp/BiomSharp/tree/master/Demos/BiomStudio#readme" target="_blank">Windows-forms demo</a> of some of the functionality.




