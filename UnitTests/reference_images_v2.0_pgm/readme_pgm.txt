This tar archive contains Version 2.0 of the reference image dataset
used for testing the compliance of Wavelet Scalar Quantization (WSQ)
image compression and decompression software implementations.

The specification is available for free download from:
	https://www.fbibiospecs.org/biometric_specs.html
		https://www.fbibiospecs.org/docs/WSQ_Gray-scale_Specification_Version_3_1_Final.pdf
			[Please note the Change History on page 2, which spells
			out the changes from prior versions of the Specification.]

Complete and up-to-date information on the compliance testing, including
instructions for processing these images with your WSQ software to generate
a submission to be certified as compliant with the specification, is
maintained at:
	http://www.nist.gov/itl/iad/ig/wsq.cfm

This is the PGM-format version of tar archive, downloadable from:
	http://nigos.nist.gov:8080/wsq/reference_images_v2.0_pgm.tar
The images in it that are to be WSQ-compressed are in the Portable
Graymap format, which is documented at:
	http://netpbm.sourceforge.net
	http://en.wikipedia.org/wiki/Netpbm_format

There's also a raw-format version of the tar archive at:
	http://nigos.nist.gov:8080/wsq/reference_images_v2.0_raw.tar
The imagery is the same as that in the PGM-format version, except
that images that are to be WSQ-compressed are in a headerless raster
format. The raw-format version is being phased out in favor of the
PGM-format version, but at the time of this writing, both are available.

Version 2.0 of the reference image dataset is a superset of the
previously-available dataset. It has been expanded to include
slap, live-scan, and low- and medium-quality rolled fingerprint
images.

The integrity of the files in this tar archive can be verified using
the MD5 checksums in:
	wsq_v2.0_pgm.md5
