import React from 'react';
import {Link, useLocation} from 'react-router-dom';

const navigation = [
    { name: 'Notifications', href: '/notifications' },
    { name: 'Account Settings', href: '/accountSettings' },
    { name: 'Proposed Positions', href: '/positions'}
]

/**
 * Element for displaying and linking elements placed in header.
 * @constructor
 */
function Navigation () {
    let thisPathname = useLocation().pathname;
    return (
        <div>
            <div className="hidden md:block">
                <div className="ml-10 flex items-baseline space-x-4">
                    {navigation.map((item, key) => (
                        <div key={key} className="text-gray-300 hover:bg-gray-700 hover:text-white">
                            <Link
                                to={item.href}
                                className={`px-3 py-2 rounded-md text-sm font-medium 
										${item.href === thisPathname
                                    ? "bg-gray-900 text-white"
                                    : "text-gray-300 hover:bg-gray-700 hover:text-white"
                                }`}>
                                {item.name}
                            </Link>
                        </div>
                    ))}
                </div>
            </div>
            <div className="block md:hidden">
                <div className="ml-4 flex items-baseline space-x-4">
                    <svg xmlns="http://www.w3.org/2000/svg" className="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="white" strokeWidth={2}>
                        <path strokeLinecap="round" strokeLinejoin="round" d="M4 6h16M4 12h16M4 18h16" />
                    </svg>
                </div>
            </div>
        </div>
    );
}

export default Navigation;