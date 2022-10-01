![Logo](https://github.com/BiomSharp/BiomSharp/blob/master/.branding/logo/github_logo.png)
######
BiomSharp, or 'Biom#', is a set of C# libraries and demo applications that provide easier and more consistent management of person biometric data, as well as an abstraction framework for working with various biometric sensors. Some of the biometric functions are provided as C# class libraries that interoperate with C/C++ native libraries.

## Motivation

Faced with client requirements to provide fingerprint verification and identification of persons, while maintaining a high degree of vendor independence as regards scanning devices and SDK's, we have created a C# framework to abstract the processes and data of biometric capture. The framework is written with the intent to provide multiple biometric modalities, although it is currently developed for fingerprint and hand-palm capture.

The framework also provides functionality required for biometric analysis separate from vendor SDK's, including minutiae detection, extraction, and print quality measures, mainly by interfacing to existing open-source libraries for biometric functions, image-format conversion, image-processing and other utilities. The framework provides access to biometric scanners functions via a C# 'wrapper' of the corresponding functions in the SDK, the functionality abstracted and exposed through a set of standard interfaces defined in the framework core library. The ultimate goal in all instances of such wrappers is to use the underlying SDK only for print-capture, while the rest of the process is present in the framework itself. In cases where C# interoperability with underlying C/C++ libraries are required, this framework endeavours to provide such functionality 'in-the-box', or a rewrite in pure C#.

## Why C#?

Most of our clients require deployments on the Microsoft Windows operating system. Also, as most biometric vendors provide mainly C++ and nowadays, C# SDK's, it has become our path of least-resistance. Also, it is our experience that using a memory-managed framework/language leads to higher quality solutions and faster delivery times. Add to this the fact that most application developers today are less experience in C/C++, makes this type of framework a more attractive alternative.

## License

[MIT](https://github.com/BiomSharp/BiomSharp/blob/master/LICENSE.txt)


