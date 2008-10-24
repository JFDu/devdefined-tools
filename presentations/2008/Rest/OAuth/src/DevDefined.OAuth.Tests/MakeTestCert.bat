REM *****************************************************************************
REM For the RSASSA-PKCS1-v1_5 algorithm in OAuth you'll need to use a certificate.
REM This batch file canbe used for generation test certs.
REM
REM Step 1 uses the Certificate Creation Tool (makecert.exe) to create a self signed 
REM X.509 certificate called testcert.cer and the corresponding private key. 
REM 
REM Step 2 uses the pvk2pfx Tool (pvk2pfx.exe) to create a Personal Information 
REM Exchange (PFX) file from a CER and PVK file. The PFX contains both your public 
REM and private key.
REM
REM See http://code.google.com/support/bin/answer.py?answer=71864&topic=12142 for further
REM hints.
REM
REM *****************************************************************************

makecert -r -pe -n "CN=Test Certificate" -sky exchange -sv testcert.pvk testcert.cer
pvk2pfx -pvk testcert.pvk -spc testcert.cer -pfx testcert.pfx