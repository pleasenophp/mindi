.DEFAULT_GOAL := build

restore:
	$(NUGET) restore

rebuild:
	$(BUILD_TOOL) /p:Configuration=$(BUILD_TARGET) /t:rebuild mindi-core.sln

build:
	$(BUILD_TOOL) /p:Configuration=$(BUILD_TARGET) /t:build mindi-core.sln

test:
	$(NUNIT) mindi-tests/bin/$(BUILD_TARGET)/mindi-tests.dll

