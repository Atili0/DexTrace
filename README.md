# Dexrm Trace

Project created by Atilio Rosas (Deloitte Labs.). This project is used for trace all our plugin and workflow. We are creating some javascript function too.

## ***Description***

Project  
1. [Dex.Trace](https://github.com/Atili0/DexTrace/tree/master/Dex.Trace"Dex.Trace")  

Test  
2. [PL_TRACE_TEST](https://github.com/Atili0/DexTrace/tree/master/PL_TRACE_TEST"PL_TRACE_TEST")  
3. [TRACE_TEST](https://github.com/Atili0/DexTrace/tree/master/TRACE_TEST"TRACE_TEST")  
4. [Solution](https://github.com/Atili0/DexTrace/tree/master/WR_Dx_Trace/solution)

## What this project create in D365?

    The solution has exported with SolutionExported tool in SDK. 

In Dynamics 365 will create tow entities.

 - Trace
 - Trace Config 

![enter image description here](http://www.dexrm.com/wp-content/uploads/2018/07/2018-07-13_13h04_39.png)

> Trace  

Here you will see all your trace saved.

**Field**
**Trace Parameter**  
If you have some error in some method, here you can find the parameter in this methos.
**Info Message**  
Save all you were configurated in the info method  
**Debug Message**  
Save all you were configurated in the debug method  

	
	
![enter image description here](http://www.dexrm.com/wp-content/uploads/2018/07/2018-07-13_13h08_45.png)

 > Trace Config  
 
You want to configurate what do you want to do in the trace
	
**Fields**
**Name:**  Here you need to paste the exact name of your class.  
**Is Debug Enabled?:**  This field has two options:  
Yes(Sí) -> The trace will work and save all messaje in the debug method  
No -> Does not work.  
**Is Info Enabled?** This field has two options  
Yes(Sí) -> The trace will save all message in the info method  
No -> The plugin does not work  
**Show message client?**  
Yes(Sí) -> The trace will show some message to the client  
No -> Does not work.  

![enter image description here](http://www.dexrm.com/wp-content/uploads/2018/07/2018-07-13_13h09_32.png)
<!--stackedit_data:
eyJoaXN0b3J5IjpbMTYxNzI0NTU3NiwxNjE5NzQxMTM2LDEzMT
I0MDc1OTNdfQ==
-->