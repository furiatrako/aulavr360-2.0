Thanks for purchase ULogin system.

Video Tutorial is here: https://www.youtube.com/watch?v=ZEZTZEJ2bgw (please read the notes below, due this turial cul be from a last version of asset).

this tutorial was for version 1.2, basically are the same steps for new version, just do not forget to upload the new files in host:

-bl_Ban.php
-bl_BanList.php
-bl_GetIP.php

From version 1.4:---------------------------------------------------------------------------------------------

From version 1.4 forward all the url of php in your host are in one place located in: Assets -> ULogin System -> Resources -> LoginDataBase
in this asset you only need put the url of directory where your php scripts are located example: 'http://www.mydomain.com/php/' (note that need the absolute url with the http or https)
if you have change the name of php script be sure of update this in the LoginDataBase script names also.

QA:-----------------------------------------------------------------------------------------------------------

Q: Where change the scene when login?
A: You can change the name of scene in Assets -> ULogin System -> Resources -> LoginDataBase -> (Variable Name) "OnLoginLoadLevel"

Q: When login just show me a message like: Login Done|string|0|0|0|0 and doesn't anything more.
A:for fix this you will need replace the script bl_Login.cs with bl_LoginInverse.cs

NOTE:---------------------------------------------------------------------------------------------------------
- if you are updating version (1.1 or 1.2) with this 1.3, you need update bl_SaveInfo.php and bl_Login.php in your host 
  with the new one in the package.

Any problem or question please you can do it here: 
Email Form: http://www.lovattostudio.com/en/support/
Forum: http://lovattostudio.com/Forum/index.php