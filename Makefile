.DEFAULT_GOAL := build
include config.mk
export

restore:
	$(MAKE) -C solution restore
rebuild:
	$(MAKE) -C solution rebuild
build:
	$(MAKE) -C solution build
test:
	$(MAKE) -C solution test

install:
ifeq ($(DEPLOY_DIR),)
	$(error Deploy dir is not set in config.mk)
endif
	@cp solution/mindi/bin/Debug/mindi.dll $(DEPLOY_DIR)/mindi.dll
	@cp solution/mindi/bin/Debug/mindi-interop.dll $(DEPLOY_DIR)/mindi-interop.dll
	@cp solution/mindi-unity/bin/Debug/mindi-unity.dll $(DEPLOY_DIR)/mindi-unity.dll
	@cp solution/mindi/bin/Debug/minioc.dll $(DEPLOY_DIR)/minioc.dll	

merge-master:
	git checkout unity4 && git merge master && git push
	git checkout unity5 && git merge master && git push

