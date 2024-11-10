import { Navbar as NextUINavbar, NavbarBrand, NavbarContent } from '@nextui-org/navbar';
import { Link } from '@nextui-org/link';
import NextLink from 'next/link';
import { IconBrandGithub, IconDotsVertical } from '@tabler/icons-react';
import { Button } from '@nextui-org/button';
import React from 'react';
import { Divider } from '@nextui-org/divider';
import { Popover, PopoverContent, PopoverTrigger } from '@nextui-org/popover';

import { Logo } from '@/components/icons';
import InfoButton from '@/components/navbar/InfoButton';
import SettingsButton from '@/components/navbar/SettingsButton';
import { useScreenSize } from '@/hooks/useScreenSize';
import WsStatus from '@/components/navbar/wsStatus/WsStatus';
import MyTooltip from '@/components/MyTooltip';

const GhButton = ({ tooltipPlacement = 'bottom' }) =>
  (
    <MyTooltip content={'GitHub'} placement={tooltipPlacement}>
      <Button isIconOnly as={Link} href={'https://github.com/MP3Martin/GameInv'} isExternal={true}
              size={'md'} variant={'light'}>
        <IconBrandGithub className="text-default-500" size={28} />
      </Button>
    </MyTooltip>
  );

const NavbarButtons = ({ tooltipPlacement = 'bottom' }) =>
  (
    <>
      <GhButton tooltipPlacement={tooltipPlacement} />
      <InfoButton tooltipPlacement={tooltipPlacement} />
      <SettingsButton tooltipPlacement={tooltipPlacement} />
    </>
  );

const MobileNavbarButtons = () => {
  return (
    <>
      <Popover>
        <PopoverTrigger>
          <Button isIconOnly size={'md'} variant={'light'}>
            <IconDotsVertical className="text-default-500" size={28} />
          </Button>
        </PopoverTrigger>
        <PopoverContent className={'popover-fix-z-index'}>
          <div className={'flex flex-col gap-1'}>
            <NavbarButtons tooltipPlacement={'left'} />
          </div>
        </PopoverContent>
      </Popover>
    </>
  );
};

export const NavBar = () => {
  const screenSize = useScreenSize();

  return (
    <NextUINavbar height={'3.2rem'} isBordered={true} maxWidth="full" position="sticky">
      <NavbarContent className="basis-1/5 sm:basis-full max-sm:ml-[-18px]" justify="start">
        <NavbarBrand className="gap-3 max-w-fit">
          <NextLink className="flex justify-start items-center gap-1" href="/">
            <Logo size={screenSize === 'xxs' ? 20 : 36} />
            <p className={'font-bold text-inherit'}
               style={{ fontSize: ((screenSize === 'xxs') ? '10px' : null) }}>GameInv</p>
          </NextLink>
        </NavbarBrand>
        <div className="hidden lg:flex gap-4 justify-start ml-2">
          {/* */}
        </div>
      </NavbarContent>

      <NavbarContent
        className="flex basis-1/5 sm:basis-full gap-1 max-sm:mr-[-18px]"
        justify="end"
      >
        <WsStatus className={'mr-[6px]'} />
        <Divider className={'h-[75%] w-[2px] bg-default-300'} orientation="vertical" />
        {(screenSize === 'xs' || screenSize === 'xxs') ? <MobileNavbarButtons /> : <NavbarButtons />}
      </NavbarContent>
    </NextUINavbar>
  );
};
