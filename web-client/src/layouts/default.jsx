import { Link } from '@nextui-org/link';

import { Head } from './head';

import { NavBar } from '@/components/navbar/NavBar';

function footerLink (text, link) {
  return <Link
    isExternal
    className="flex items-center gap-1 text-current"
    href={link}
  >
    <p className="text-primary">{text}</p>
  </Link>;
}

export default function DefaultLayout ({ children }) {
  return (
    <div className="relative flex flex-col h-full flex-grow">
      <Head />
      <NavBar />
      <main className="container mx-auto max-w-7xl px-6 flex-grow pt-8">
        {children}
      </main>
      <footer className="w-full flex items-center justify-center pt-3">
        <p className="text-default-600">Powered by</p><span>&nbsp;</span>
        {footerLink('NextUI', 'https://nextui.org/')}
        <span>&nbsp;&&nbsp;</span>
        {footerLink('Next.JS', 'https://nextjs.org/')}
      </footer>
    </div>
  );
}
