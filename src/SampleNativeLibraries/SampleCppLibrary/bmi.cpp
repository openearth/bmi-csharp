// SampleCppLibrary.cpp : Defines the exported functions for the DLL application.
//

#include <stdio.h>
#include "bmi.h"
#include <string.h>


double current_time = 0.0;
double time_step = 1.0;
logger_callback logger;

double* v1 = new double[10];


enum LEVEL { ALL, DEBUG, INFO, WARN, ERROR, FATAL, OFF };

// TODO: move to a separate BMI C/C++ project.

void log(char* message)
{
	if (logger == NULL)
	{
		printf(message);
	}
	else
	{
		int level = (int)INFO;
		printf("%d : %s\n", level, message);
		printf("%d\n", logger);
		logger(&level, message);
	}
}

BMI_API int initialize(const char* config_file)
{
	log("initializing ...\n");
	return 0;
}

BMI_API int update(const double& dt)
{
	log("updating ...\n");

	if (dt == -1)
	{
		current_time += time_step;
	}
	else
	{
		current_time += dt;
	}

	return 0;
}

BMI_API int finalize()
{
	log("finalizing ...\n");
	return 0;
}

BMI_API void set_logger(logger_callback logger)
{
	::logger = logger;
}

BMI_API void get_start_time(double& t)
{
	t = 0.0;
}

BMI_API void get_end_time(double& t)
{
	t = 5.0;
}

BMI_API void get_current_time(double& t)
{
	t = current_time;
}

BMI_API void get_time_step(double& dt)
{
}

BMI_API void get_var_count(int& count) // non-BMI
{
	count = 1;
} 

BMI_API void get_var_name(const int index, char* variable) // non-BMI
{
	strcpy(variable, "v1");
} 

BMI_API void get_var_shape(const char* variable, int* shape)
{
}

BMI_API void get_var_rank(const char* variable, int& rank)
{
}

BMI_API void get_var_values(const char* variable, void* values)
{
	v1[0] = 1.0;
	v1[1] = 2.0;

	values = &v1;
}

BMI_API void get_var_values2(void*& values)
{
	v1[0] = 1.0;
	v1[1] = 2.0;

	values = &v1;
}

BMI_API void set_var_values(const char* variable, void* values)
{
	if(values != &v1)
	{
		log("copy values");
	}
	else
	{
		log("set values using pointer");
	}
}

BMI_API void get_var_values_slice(const char *name, int* start, int* stop, int* step, void *values)
{
}

BMI_API void set_var_values_slice(const char *name, int* start, int* stop, int* step, void *values)
{
}

