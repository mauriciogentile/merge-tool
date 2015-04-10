Usage:

	MergeToolConsole [filePath] [confidence]

Options:
	
	filePath	Path to a json file. If not specified ".\test\Contacts1.json" will be used.
	confidence	A value between 0 and 1. If not specified 0.7 will be used.

Examples:

	MergeToolConsole
	MergeToolConsole "C:\test\Contacts1.json"
	MergeToolConsole "C:\test\Contacts2.json" 0.5