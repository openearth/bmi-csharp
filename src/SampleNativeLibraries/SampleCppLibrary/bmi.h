// The following ifdef block is the standard way of creating macros which make exporting 
// from a DLL simpler. All files within this DLL are compiled with the SAMPLECPPLIBRARY_EXPORTS
// symbol defined on the command line. This symbol should not be defined on any project
// that uses this DLL. This way any other project whose source files include this file see 
// SAMPLECPPLIBRARY_API functions as being imported from a DLL, whereas this DLL sees symbols
// defined with this macro as being exported.
#ifdef SAMPLECPPLIBRARY_EXPORTS
#define BMI_API __declspec(dllexport)
#else
#define BMI_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C" {
#endif

	typedef void(*logger_callback)(int*, char*);

	// control

	BMI_API int initialize(const char* config_file);

	BMI_API int update(const double& dt);

	BMI_API int finalize();

	// logging

	BMI_API void set_logger(logger_callback logger);

	// time

	BMI_API void get_start_time(double& t);

	BMI_API void get_end_time(double& t);

	BMI_API void get_current_time(double& t);

	BMI_API void get_time_step(double& dt);

	// variable

	BMI_API void get_var_count(int& count);

	BMI_API void get_var_name(const int index, char* variable);

	BMI_API void get_var_shape(const char* variable, int* shape);

	BMI_API void get_var_rank(const char* variable, int& rank);


	BMI_API void get_var_values(const char* name, void *values);

	BMI_API void set_var_values(const char* name, void *values);

	BMI_API void get_var_values2(void*& values);

	BMI_API void get_var_values_slice(const char *name, int* start, int* stop, int* step, void *values);

	BMI_API void set_var_values_slice(const char *name, int* start, int* stop, int* step, void *values);
}