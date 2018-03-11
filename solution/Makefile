.DEFAULT_GOAL := build

restore:
	mono nuget.exe restore

rebuild:
	xbuild /p:Configuration=$(BUILD_TARGET) /t:rebuild mindi-core.sln

build:
	xbuild /p:Configuration=$(BUILD_TARGET) /t:build mindi-core.sln

test:
	$(NUNIT) mindi-tests/bin/$(BUILD_TARGET)/mindi-tests.dll

