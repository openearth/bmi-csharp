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

	// TODO: move to a separate BMI C/C++ project.

	BMI_API void initialize(const char* config_file);

	BMI_API void update(const double& dt);

	BMI_API void finalize();

	BMI_API void get_start_time(double& t);

	BMI_API void get_end_time(double& t);

	BMI_API void get_current_time(double& t);

	BMI_API void get_time_step(double& dt);

	BMI_API void get_string_attribute(const char& name, char& value);

	BMI_API void get_n_variables(int& count); // non-BMI

	BMI_API void get_variable_name(const int index, char* variable); // non-BMI

	BMI_API void get_var_shape(const char& variable, int* shape);

	BMI_API void get_var_rank(const char& variable, int& rank);

	BMI_API void get_1d_double(const char& variable, double** values);

	BMI_API void get_1d_int(const char& variable, int** values);

	BMI_API void get_2d_int(const char& variable, int*** values);

	BMI_API void set_1d_double_at_index(const char& variable, const int& index, double& value);

	BMI_API void set_logger(logger_callback logger);
}