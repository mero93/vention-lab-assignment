'use client';

import { PlusCircle, Search, BrushCleaning } from 'lucide-react';
import { useRouter, usePathname, useSearchParams } from 'next/navigation';
import { Button } from './ui/button';
import { Input } from './ui/input';
import { useState } from 'react';

export default function Header() {
  const router = useRouter();
  const pathname = usePathname();
  const searchParams = useSearchParams();

  const [searchTerm, setSearchTerm] = useState<string>('');

  const updateQuery = (query: string) => {
    const params = new URLSearchParams(searchParams.toString());

    if (query) {
      params.set('title', query);
    } else {
      params.delete('title');
    }

    router.push(`${pathname}?${params.toString()}`);
  };

  const clearQuery = () => {
    setSearchTerm('');
    updateQuery('');
  };

  return (
    <header className="flex w-full justify-center border-b border-border bg-card py-6">
      <div className="flex w-full max-w-400 items-center justify-between">
        <div className="flex items-center gap-2">
          <Button size="icon" variant="outline" onClick={() => updateQuery(searchTerm)}>
            <Search />
          </Button>
          <Input
            placeholder="Search by title..."
            className="w-45"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
            onKeyDown={(e) => e.key === 'Enter' && updateQuery(searchTerm)}
          />
          <Button size="icon" variant="info" onClick={clearQuery}>
            <BrushCleaning />
          </Button>
        </div>
        <h1 className="text-xl">Media Inventory Manager</h1>

        <div className="flex items-center gap-9">
          <Button variant="default" size="lg" className="gap-2 font-bold">
            <PlusCircle className="size-6" />
            Add Product
          </Button>
        </div>
      </div>
    </header>
  );
}
